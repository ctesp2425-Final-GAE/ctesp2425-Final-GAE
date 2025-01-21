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
     - **POST** `/reservations` - Eduardo
     - **PUT** `/reservations/{id}` - Camila
     - **DELETE** `/reservations/{id}` - Mariana **FEITO**
     - **GET** `/reservations?date={date}` - Camila
   - Garantir validações:
     - Verificação de conflitos de horários para reservas.

4. **Base de Dados** - Mariana
   - Criar a estrutura inicial da tabela conforme o modelo fornecido.
   - Configurar o SQL Server utilizando o **Vagrant**. (Apenas vagrant file)

5. **Swagger** - Camila
   - Documentar todos os endpoints no Swagger para testes e visualização.


### **Virtualização e Containerização**
6. **Configuração do Ambiente** - Guilherme **FEITO**
   - Criar um **Dockerfile** para a API. - Guilherme **FEITO**
   - Configurar comunicação entre API e banco de dados:
     - Via container Docker. - Guilherme **FEITO**


### **Testes e Garantia de Qualidade**
7. **Testes Unitários**
   - Implementar testes unitários usando **xUnit** para: - Eduardo e Mariana
     - Criação de reservas.
     - Verificação de conflitos de horários.
     - Operações de listagem e atualização.
   - Cobrir pelo menos 80% do código.

8. **Qualidade do Código** - Guilherme **EM PROCESSO**
   - Integrar e configurar **SonarQube** para análise de qualidade.


### **Pipeline Automatizado** - Mariana
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
