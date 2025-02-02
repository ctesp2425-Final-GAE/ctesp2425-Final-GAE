# ctesp2425-Final-GAE

## Divisao de Tarefas
Aqui está uma lista de tarefas divididas para o projeto de acordo com os requisitos descritos:

### **Planejamento Inicial**

1. **Configuração do Repositório** - Guilherme **FEITO**

   - Criar um repositório no GitHub com o nome no formato especificado.
   - Configurar os branches `master` e `develop`.
   - Compartilhar o repositório com o docente.

2. **Documentação**

   - Criar um plano de desenvolvimento inicial.
   - Especificar tecnologias, bibliotecas e frameworks que serão utilizados.
   - Detalhar suposições e referências externas no documento.


### **Desenvolvimento do Backend**

3. **Implementação da API REST**

   - Criar endpoints:
     - **GET** `/reservations` - Guilherme **FEITO**
     - **GET** `/reservations/{id}` - Mariana **FEITO**
     - **POST** `/reservations` - Eduardo **FEITO**
     - **PUT** `/reservations/{id}` - Camila
     - **DELETE** `/reservations/{id}` - Mariana **FEITO**
     - **GET** `/reservations?date={date}` - Camila
   - Garantir validações:
     - Verificação de conflitos de horários para reservas.

4. **Base de Dados** - Mariana **FEITO**
   - Criar a estrutura inicial da tabela conforme o modelo fornecido.
   - Configurar o SQL Server utilizando o **Vagrant**. (Apenas vagrant file)

5. **Swagger** - Camila
   - Documentar todos os endpoints no Swagger para testes e visualização.


### **Virtualização e Containerização**
6. **Configuração do Ambiente** - Guilherme **FEITO**
   - Criar um **Dockerfile** para a API. - Guilherme **FEITO**
   - Configurar comunicação entre API e base de dados:
     - Via container Docker. - Guilherme **FEITO**


### **Testes e Garantia de Qualidade**
7. **Testes Unitários**
   - Implementar testes unitários usando **xUnit** para: - Eduardo e Mariana
     - Criação de reservas.
     - Verificação de conflitos de horários.
     - Operações de listagem e atualização.
   - Cobrir pelo menos 80% do código.

8. **Qualidade do Código** - Guilherme **FEITO**
   - Integrar e configurar **SonarQube** para análise de qualidade.


### **Pipeline Automatizado** - Mariana **FEITO**
9. **Jenkins**
   - Configurar um pipeline para:
     - Build do código.
     - Execução de testes unitários.
     - Geração de relatórios de qualidade.
     - Empacotamento e deployment em container Docker.
   - Garantir conectividade entre Jenkins, API e base de dados.


### **Entrega Final**

10. **Relatório** - A definir
    - Elaborar um relatório detalhado com:
      - Processo de desenvolvimento.
      - Desafios enfrentados.
      - Prints do Swagger e execução do pipeline.
      - Relatórios do SonarQube.
    - Garantir entre 10 a 20 páginas.

11. **Revisão Final** - A definir
    - Testar todos os aspectos do sistema.
    - Validar que todas as funcionalidades estão operacionais antes do prazo.

Essas tarefas podem ser atribuídas entre os membros do grupo, garantindo que todos os aspectos sejam cobertos com eficiência. Caso precise de mais detalhes sobre alguma tarefa, posso ajudar a expandir!

---

## Funcionaliadades e Documentação

### 18/01/2025 - Guilherme
Associação de projetos a solução, criação de pastas.
Configuração do Docker com Dockerfile e docker-compose para dar start a BD.

#### Docker configração
Rodar docker compose para gerar os serviços necessarios: 
`docker-compose -p restaurant-api up --build -d`


### 19/01/2025 - Guilherme
Introduzido 2 models: Table (Mesas) e Reservation (Reservas) e criado os respetivos controllers. 

Neste passo foi introduzido as migrações para a base de dados e a conection string guardada dentro dum .env para segurança da aplicação.

#### GetAllReservations
Endpoint que permite ir buscar todas as reservas à base de dados.

#### Criar migrações
**Criar ficheiro .env atraves do .env.example**.

*Criar Migrações*:
``dotnet ef migrations add initalMigrate``

*Gerar BD*:
``dotnet ef database update``

*Testar API*:
``dotnet run``

### 20/01/2025 - Mariana

### Endpoints Disponíveis
#### **GET** `/api/reservations/{id}`

Obtém os detalhes de uma reserva específica pelo ID.

- **Método**: `GET`
- **Rota**: `/api/reservations/{id}`

#### **Descrição**
Este endpoint retorna os detalhes de uma reserva com base no ID fornecido. Caso o ID não seja encontrado, será retornada uma mensagem de erro.

#### **Exemplo de Requisição**
```http
GET /api/reservations/1
```

#### **Respostas**
- **Sucesso** (`200 OK`):
  ```json
  {
      "id": 1,
      "customerName": "John Doe",
      "reservationDate": "2025-01-21",
      "reservationTime": "19:00:00",
      "tableNumber": 5,
      "numberOfPeople": 4,
      "createdAt": "2025-01-20T15:30:00"
  }
  ```
- **Erro** (`404 Not Found`):
  ```json
  {
      "message": "Reservation not found"
  }
  ```

---

### **DELETE** `/api/reservations/{id}`

Apaga uma reserva específica pelo ID.

- **Método**: `DELETE`
- **Rota**: `/api/reservations/{id}`

#### **Descrição**
Este endpoint permite eliminar uma reserva existente pelo ID fornecido. Caso o ID não exista, será retornada uma mensagem de erro.

#### **Exemplo de Requisição**
```http
DELETE /api/reservations/1
```

#### **Respostas**
- **Sucesso** (`200 OK`):
  ```json
  {
      "message": "Reservation deleted successfully"
  }
  ```
- **Erro** (`404 Not Found`):
  ```json
  {
      "message": "Reservation not found"
  }
  ```

---

### Configuração do Vagrant

#### **Vagrantfile**
A configuração abaixo define o ambiente necessário para hospedar a base de dados SQL Server:

```ruby
Vagrant.configure("2") do |config|
    config.vm.box = "gusztavvargadr/sql-server"
    config.vm.box_version = "2019.2102.2409"
  
    # Configuração de rede para permitir acesso externo ao SQL Server
    config.vm.network "forwarded_port", guest: 1433, host: 1433, auto_correct: true
  
    # Configuração de memória RAM
    config.vm.provider "virtualbox" do |vb|
      vb.memory = "2048"
    end
end
```


### 20/01/2025 - Mariana

### Pipeline Automatizado

### **Jenkins**

Foi implementado um pipeline automatizado utilizando Jenkins, com as seguintes funcionalidades:

#### **Checkout do código**:
- Recupera o código do repositório no GitHub.

#### **Restaurar dependências**:
- Instala as dependências do projeto utilizando o comando `dotnet restore`.

#### **Build do projeto**:
- Realiza a construção do projeto em modo Release.

#### **Publicação da aplicação**:
- Publica a aplicação em um diretório específico para deployment futuro.

#### **Exemplo de Pipeline**

```groovy
pipeline {
    agent any

    stages {
        stage('Checkout') {
            steps {
                echo 'Fazendo o checkout do código...'
                git branch: 'master', url: 'https://github.com/ctesp2425-Final-GAE/ctesp2425-Final-GAE.git'
            }
        }

        stage('Restore') {
            steps {
                echo 'Restaurando dependências do projeto...'
                sh 'dotnet restore ./RestaurantReservationAPI/'
            }
        }

        stage('Build') {
            steps {
                echo 'Construindo o projeto...'
                sh 'dotnet build ./RestaurantReservationAPI/ --configuration Release'
            }
        }

        stage('Publish') {
            steps {
                echo 'Publicando a aplicação...'
                sh 'dotnet publish ./RestaurantReservationAPI/ --configuration Release --output ./RestaurantReservationAPI/publish'
            }
        }
    }

    post {
        success {
            echo 'Pipeline executado com sucesso!'
        }
        failure {
            echo 'Falha durante a execução do pipeline.'
        }
    }
}
```

### 21/01/2025 - Camila

## UpdateReservations e GetReservationsByDate

Desenvolvi dois endpoints, o UpdateReservations e GetReservationsByDate. Porém ainda não foi testado.

## 23/01/2025 - Camila

## Testes UpdateReservations e GetReservations(DateTime? date, string? customerName)

Testei o UpdateReservations e ele funcionou perfeitamente.

Fiz alteração no GetReservationsByDate, pois estava a me dar erro. O erro era pois eu tinha dois endpoints na mesma route. Sendo assim, para resolver isso, coloquei parametros no GetReservations(DateTime? date, string? customerName).
Depois disso testei o endpoint e ele estava a funcionar.

## Swagger

Segui o link que o professor fornecer no slide da aula sobre o Swagger. (https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-8.0&tabs=visual-studio-code)

Resumindo, fiz a instalação do package Swashbuckle.AspNetCore

"dotnet add TodoApi.csproj package Swashbuckle.AspNetCore -v 6.6.2"

Depois no Program.cs adicionei as seguintes linhas de códigos

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

app.UseSwagger();
app.UseSwaggerUI();

Assim, o Swagger já estava a funcionar e já era possivel realizar os testes dos endpoints.

### 23/01/2025 - Eduardo

## Endpoints Disponíveis

### **POST** `/api/reservations`

Cria uma nova reserva no sistema.

- **Método**: POST
- **Rota**: /api/reservations

#### **Descrição**
Este endpoint permite criar uma nova reserva fornecendo informações como nome do cliente, data, hora, número de pessoas e número da mesa.

#### **Exemplo de Requisição**
```http
POST /api/reservations  
```
 
#### **Respostas**
**Sucesso** (`201 Created`):
```json
{
	"id": 1,
	"customerName": "John Doe",
	"reservationDate": "2025-01-24T00:00:00",
	"reservationTime": "19:00:00",
	"tableNumber": 5,
	"numberOfPeople": 4,
	"createdAt": "0001-01-01T00:00:00"
}
  ```
**Erro** (`400 Bad Request`):
```json
{  
    "message": "Invalid reservation data"  
}  
```

### 23/01/2025 - Guilherme
Sonarquebe e test coverge:

#### Codigo executado:
```bash
dotnet sonarscanner begin /k:"dsos" /d:sonar.host.url="http://localhost:9001"  /d:sonar.token="sqp_4fead643e4ca5a7a6d61bef0e6934428e2d25321" /d:sonar.cs.vscoveragexml.reportsPaths=coverage.xml
dotnet build --no-incremental
dotnet-coverage collect 'dotnet test' -f xml  -o 'coverage.xml'
dotnet sonarscanner end /d:sonar.login="sqp_4fead643e4ca5a7a6d61bef0e6934428e2d25321"
```

## 26/01/2025 - Camila

Realizei algumas alterações no código GetReservations(DateTime? date, string? customerName), pois estava só a filtrar por data. Agora filtra por data e hora da reserva.

Também fiz alterações no UpdateReservation. Coloquei validações para que não consigam editar para colocar uma mesa acima do tanto de mesas existentes. E validação para não conseguir editar para colocar uma mesa na mesma data e horario de outra reserva.

## 29/01/2025 - Camila

Para realizar a documentação tive que adicionar no program.cs a criação da documentação do swagger. E adicionei no RestauranteReservationAPI.csproj para indicar ao compilador que ele deve criar um arquivo XML contendo a documentação.

Depois disso, realizei a documentação no código do Swagger de todos os endpoints que já estavam desenvolvidos. 



### 01/02/2025 - Eduardo e Mariana

**Testes Unitários**

## GetReservations_ReturnsAllReservations

Verifica se todas as reservas são retornadas corretamente.

## Método: GET

Descrição: Testa a recuperação de todas as reservas presentes na base de dados.

## Exemplo de Requisição

var result = controller.GetReservations() as OkObjectResult;

## Respostas

Sucesso (200 OK):

```json

  {
    "id": 1,
    "customerName": "John Doe",
    "reservationDate": "2025-02-01",
    "reservationTime": "12:00:00",
    "tableNumber": 1,
    "numberOfPeople": 4
  },
  {
    "id": 2,
    "customerName": "Jane Smith",
    "reservationDate": "2025-02-02",
    "reservationTime": "18:00:00",
    "tableNumber": 2,
    "numberOfPeople": 2
  }
  ```

## GetReservationById_ReturnsCorrectReservation

Verifica se uma reserva específica é retornada pelo ID.

## Método: GET

Descrição: Testa a recuperação de uma reserva usando o ID.

## Exemplo de Requisição

var result = controller.GetReservationById(1) as OkObjectResult;

## Respostas

Sucesso (200 OK):

```json

{
  "id": 1,
  "customerName": "John Doe",
  "reservationDate": "2025-02-01",
  "reservationTime": "12:00:00",
  "tableNumber": 1,
  "numberOfPeople": 4
}

Erro (404 Not Found):
{
  "message": "Reservation not found"
}
```

## CreateReservation_AddsReservation

Verifica se uma nova reserva é criada com sucesso.

## Método: POST

Descrição: Testa a adição de uma nova reserva.

## Exemplo de Requisição

var result = controller.CreateReservation(newReservation) as CreatedAtActionResult;

## Respostas

Sucesso (201 Created):

```json

{
  "id": 3,
  "customerName": "Alice Brown",
  "reservationDate": "2025-02-01",
  "reservationTime": "19:00:00",
  "tableNumber": 3,
  "numberOfPeople": 5
}

Erro (400 Bad Request):
{
  "message": "Invalid reservation data"
}
```

## DeleteReservation_RemovesReservation

Verifica se uma reserva é removida com sucesso.

## Método: DELETE

Descrição: Testa a exclusão de uma reserva existente.

## Exemplo de Requisição

var result = controller.DeleteReservation(1) as OkObjectResult;

## Respostas

Sucesso (200 OK):

```json

{
  "message": "Reservation deleted successfully"
}

Erro (404 Not Found):
{
  "message": "Reservation not found"
}
```

## UpdateReservation_UpdatesReservationDetails

Verifica se uma reserva é atualizada corretamente.

## Método: PUT

Descrição: Testa a atualização de informações de uma reserva.

## Exemplo de Requisição

var result = controller.UpdateReservation(1, updatedReservation) as OkObjectResult;

## Respostas

Sucesso (200 OK):

```json

{
  "id": 1,
  "customerName": "John Doe Updated",
  "reservationDate": "2025-02-01",
  "reservationTime": "13:00:00",
  "tableNumber": 2,
  "numberOfPeople": 6
}

Erro (404 Not Found):
{
  "message": "Reservation not found"
}
```

## GetReservations_FiltersByDateAndCustomerName

Verifica se o filtro por data e nome do cliente funciona corretamente.

## Método: GET

Descrição: Testa a filtragem de reservas com base na data e no nome do cliente.

## Exemplo de Requisição

var result = controller.GetReservations(DateTime.Today.AddHours(12), "John Doe") as OkObjectResult;

## Respostas

Sucesso (200 OK):

```json

  {
    "id": 1,
    "customerName": "John Doe",
    "reservationDate": "2025-02-01",
    "reservationTime": "12:00:00",
    "tableNumber": 1,
    "numberOfPeople": 4
  }
  ```

## GetReservations_FiltersByCustomerNameOnly

Verifica se a filtragem de reservas funciona corretamente com base apenas no nome do cliente.

## Método: GET

Descrição Testa a recuperação de reservas filtradas somente pelo nome do cliente.

## Exemplo de Requisição

var result = controller.GetReservations(null, "Jane Smith") as OkObjectResult;

## Respostas

Sucesso (200 OK):

```json

  {
    "id": 2,
    "customerName": "Jane Smith",
    "reservationDate": "2025-02-02",
    "reservationTime": "18:00:00",
    "tableNumber": 2,
    "numberOfPeople": 2
  }
  ```

## GetReservations_ReturnsAllReservations_WhenNoFiltersApplied

Verifica se todas as reservas são retornadas corretamente quando nenhum filtro é aplicado.

## Método: GET

Descrição: Testa a recuperação de todas as reservas sem filtros de data ou nome de cliente.

## Exemplo de Requisição

var result = controller.GetReservations(null, null) as OkObjectResult;

## Respostas

Sucesso (200 OK):

```json

  {
    "id": 1,
    "customerName": "John Doe",
    "reservationDate": "2025-02-01",
    "reservationTime": "12:00:00",
    "tableNumber": 1,
    "numberOfPeople": 4
  },
  {
    "id": 2,
    "customerName": "Jane Smith",
    "reservationDate": "2025-02-02",
    "reservationTime": "18:00:00",
    "tableNumber": 2,
    "numberOfPeople": 2
  }
  ```

### Desafios Enfrentados
**Cobertura de Testes Unitários (80%)**

Foi difícil atingir os 80% de cobertura exigidos, especialmente para validações de conflitos e integração com a base de dados.

**Compreensão do Código por Todos os Membros**

Algumas partes do código não estavam bem documentadas, o que dificultou a colaboração.

Algumas implementações (como a lógica de conflitos de reservas) foram complexas para alguns membros.

**Conflito de Rotas no GET /reservations**

Existiam dois métodos GET com assinaturas diferentes, o que gerou conflitos na API.

**SonarQube**

Dificuldades em configurar a análise de cobertura de código e garantir que os testes eram executados automaticamente no Jenkins.

### Possíveis Melhorias
Aprimorar a Escrita de Testes Unitários

Criar um guia interno sobre boas práticas de testes para facilitar a compreensão.

Melhorar a Documentação do Código

Adicionar mais comentários explicativos nas partes críticas do código.

Diferenciar os métodos alterando um deles para GET /reservations/search para evitar conflitos.

Refinar Validações no Backend

Otimizar a Integração com Jenkins e SonarQube