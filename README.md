
# Doe Bem - Back-end


## Rodando o Projeto

Clone o projeto: 

**por HTTPS**

```bash
  git clone https://github.com/lucasbaumer/DoeBem-app-back.git
```
  **por SSH**
```bash
  git clone git@github.com:lucasbaumer/DoeBem-app-back.git
```

## Entre no diretório do projeto

```bash
  cd doeBem.Presentation
```

## Configure o banco de dados local
No arquivo appsettings.json dentro da pasta doebem.presentation, altere a string de conexão para apontar para seu SQL server local

```bash
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=DoeBemDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```
## Caso utilize autenticação por usuário e senha, ajuste para:

```bash
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=DoeBemDb;User Id=seu_usuario;Password=sua_senha;TrustServerCertificate=True;"
}
```

## Restaure os pacotes e atualize o banco 
O MyDbContext está localizado na pasta Infrastructure. Para aplicar as migrations e atualizar o banco rode os comandos abaixo na raiz do projeto (doebem.presentation)

Criar migration (substitua "NomeDaMigration" por um nome que descreva as mudanças)
```bash
dotnet ef migrations add NomeDaMigration --project doeBem.Infrastructure --startup-project doeBem.Presentation
```

```bash
# Atualizar o banco aplicando a migration
dotnet ef database update --project doeBem.Infrastructure --startup-project doeBem.Presentation
```

## Se não possui a ferramenta dotnet -ef instalada: 

```bash
dotnet tool install --global dotnet-ef
```

## Inicie o servidor (dentro da solução doebem.presentation)

```bash
  dotnet run
```

## Após iniciar o projeto, acesse a interface Swagger através do seguinte link:

📌 **Nota:** Substitua `PORTA` pela porta que aparece no terminal ao rodar a aplicação.
```bash
   http://localhost:PORTA/swagger/index.html
```


## Autores

- [@Lucas Baumer](https://www.github.com/lucasbaumer)
- [@Matheus Kormann](https://www.github.com/matheuskormann)
- [@MuriloMayer](https://www.github.com/MuriloMayer)


