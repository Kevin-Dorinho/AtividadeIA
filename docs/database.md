# Banco de Dados
- Use **MySQL Connector** para conexão com o banco de dados
- Use o xampp para rodar o banco(ele já está aberto e rodando na porta 3307) não precisa de senha.

## Tabelas

- Tabela: `ALUNO`
  - Colunas: `NOME`, `RG`, `CPF`, `DATA_NASCIMENTO`, `ENDERECO`, `TELEFONE`, `EMAIL`, `CURSO`, `TURMA`
  - Relacionamentos: 
    - `CURSO` <- `ID`
    - `TURMA` <- `ID`

- Tabela: `CURSO`
  - Colunas: `ID`, `NOME`

- Tabela: `TURMA`
  - Colunas: `SERIE DA TURMA`, `TURNO`, `CURSO`.

## Relacionamentos

- `ALUNO` <- `ID` (`CURSO`)
- `ALUNO` <- `ID` (`TURMA`)