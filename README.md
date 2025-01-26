# Contato Inteligente - Chatbot

Este é um repositório de código para API RESTful e fluxo de conversação de um Chatbot criado como parte do desafio Chatbot Developer CSPS na plataforma Blip.ai.

## Descrição

O projeto consiste em um chatbot que interage com o usuário e exibe informações sobre os 5 repositórios mais antigos de C# do GitHub da Takenet. A API intermediária foi criada para consumir a API pública do GitHub e fornecer esses dados ao chatbot.

## Funcionalidades

- **Chatbot:** O chatbot segue o fluxo de conversação proposto, verifica a validação de entrada com expressões regulares e lida com erros no fluxo e integração com a API adequadamente.
- **API RESTful:** A API retorna os 5 repositórios C# mais antigos da Takenet e consome a API pública do GitHub. A resposta inclui campos como nome, descrição, link do avatar do repositório e data da última atualização.

## Endpoints da API

- **GET /repositories**: Retorna os repositórios do github de acordo com os filtros informados.

- #### Parâmetros de consulta:

- **UserName**: Nome do usuário no GitHub (obrigatório).
- **Language**: Linguagem de programação para filtrar os repositórios (opcional).
- **Order**: Direção da ordenação (ascendente ou descendente) (opcional).
- **Sort**: Critério de ordenação (ex: por data ou nome) (opcional).
- **PerPage**: Número de resultados por página (default: 5) (opcional).


### Exemplo de resposta:

```json
[
  {
    "userAvatarLink": "https://avatars.githubusercontent.com/u/4369522?v=4",
    "name": "Takenet.ScoreSystem",
    "description": "Takenet score system",
    "updatedAt": "2015-03-21T20:53:04+00:00"
  },
  ...
]
