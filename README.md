# Passwords-MD5-SALT
Trabalhando com senhas cifradas com MD5 + SALT

## Classe auxiliar para trabalhar com codificação em BASE64

Vamos criar uma classe para codificar e decodificar textos para Base64

```csharp
    public static class Base64
    {
        public static string Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
```

## Classe principal para trabalhar com Hash MD5 e SALT

O SALT é um texto que colocamos junto com a senha para o Hash gerado não fique fácil de ser quebrado, se tivermos acesso em uma base de dados com hashs de senhas MD5, podemos fazer uma comparação com uma tabela de hashs e verificar se os hashs batem, podemos agrupar hashs também para saber que são as mesmas senhas.
Agora se colocarmos um valor a mais na senha do usuário (SALT) então irá dificultar muita a quebra desses hashs.
  
```csharp
    public class PasswordHash 
    {
        private const int SALT_LENGTH = 19;

        public SaltHash GetHash(string password)
        {
            var salt = CreateSalt();
            return GetHash(password, salt);
        }

        public SaltHash GetHash(string password, string salt)
        {
            var provider = MD5.Create();
            var bytes = provider.ComputeHash(Encoding.ASCII.GetBytes(salt + password));
            var hash = Base64.Encode(salt + FormatHash(bytes));

            return new SaltHash(salt, hash); 
        }

        public string GetSalt(string passwordHashedBase64)
        {
            var data = Base64.Decode(passwordHashedBase64);
            var salt = data.Substring(0, SALT_LENGTH * 2);
            return salt;
        }

        public bool CompareHashs(string passwordHashedBase64, string password)
        {
            var salt = GetSalt(passwordHashedBase64);
            var hash = GetHash(password, salt).Hash;

            return passwordHashedBase64.Equals(hash);
        }

        private string CreateSalt()
        {
            byte[] buff = new byte[SALT_LENGTH];

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buff);

            var salt = FormatHash(buff);

            return salt;
        }

        private string FormatHash(byte[] buffer)
        {
            return BitConverter.ToString(buffer).Replace("-", "");
        } 
    }
```

Essa classe possui o método GetHash(pass) e GetHash(pass, salt) que vamos utilizar para gerar o hash, podemos chamar o metodo GetHash passando a senha digitada e ele vai criar um SALT de tamanho fixo (nesse caso de 19 Bytes) e vai gerar o hash da senha no seguinte formato (salt + hashMd5(salt + password)). Se a base de dados de hashs vazar, quem estiver com ela não vai conseguir fazer uma comparação de hashs pois ele teria que extrair o salt e como ele não sabe o tamanho do SALT que pode ser qualquer um (aqui configuramos 19, mas poderia ser qualquer valor).

## Usando a Classe PasswordHash

Para utilizar basta instanciar a classe e chamar os métodos correspondentes.

```csharp
        private void btnGerar_Click(object sender, EventArgs e)
        {
            var passwordHash = new PasswordHash();
            var retorno = passwordHash.GetHash(txtPassword.Text);

            txtSalt.Text = retorno.Salt;
            txtPasswordHash.Text = retorno.Hash; 
        }

        private void btnTestarLogin_Click(object sender, EventArgs e)
        {
            var pass = txtPassword.Text;
            var hash = txtPasswordHash.Text;

            var passwordHash = new PasswordHash();
            var retorno = passwordHash.CompareHashs(hash, pass);
             
            if (retorno)
                MessageBox.Show("Logado com sucesso");
            else
                MessageBox.Show("Usuário ou Password incorreto");
        }
```


