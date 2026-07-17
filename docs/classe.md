## Classes
- As classes devem seguir o mesmo padrão do arquivo `SistemaEscolar` com o adendo de acrescentar uma classe nova que são as turmas
- O codigo deve ser bem estruturado e organizado
- Sempre que for criar uma classe, crie um método `Mostrar()` para exibir as informações da classe

## Classes
- A classe **Alunos** deve conter `NOME`, `RG`, `CPF`, `DATA_NASCIMENTO`, `ENDERECO`, `TELEFONE`, `EMAIL`, `CURSO`, `TURMA`.
- A classe **Cursos** deve conter `ID`, `NOME DO CURSO`
- A classe **Turmas** deve conter `SERIE DA TURMA`, `TURNO`, `CURSO`.


## Método Salvar
- Este método deve salvar as informações da classe no banco de dados
- Deve usar MySQL Connector para conexão com o banco de dados