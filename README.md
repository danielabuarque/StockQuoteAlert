# StockQuoteAlert

Uma aplicação de console em C# para monitorar cotações de ativos da B3 e enviar alertas por e-mail.

---

## Sobre o Projeto

O **StockQuoteAlert** é uma ferramenta simples e eficiente que monitora a cotação de um ativo financeiro. Quando o preço do ativo ultrapassa um limite de venda ou fica abaixo de um limite de compra, a aplicação dispara um alerta por e-mail. A aplicação foi desenvolvida como um desafio para demonstrar conhecimentos em C#, tratamento de APIs, envio de e-mails e manipulação de configurações externas.

---

## Como Usar o Executável (.exe)

Para rodar a aplicação, siga estes passos simples:

### 1. Requisitos
* O arquivo `StockQuoteAlert.exe`.
* O arquivo de configurações `appsettings.json` na mesma pasta do executável.

### 2. Configurar o `appsettings.json`

Antes de rodar a aplicação, você precisa configurar os dados de acesso ao servidor de e-mail e a API de cotação. Edite o arquivo `appsettings.json` e preencha as informações necessárias.

**Exemplo de `appsettings.json`:**
```json
{
  "SmtpSettings": {
    "Server": "smtp.seuprovedor.com",
    "Port": 587,
    "Username": "seu.email@exemplo.com",
    "Password": "sua-senha-aqui"
  },
  "AlertSettings": {
    "ToEmail": "email-de-destino@exemplo.com"
  },
  "ApiSettings": {
    "BaseUrl": "[https://brapi.dev/api](https://brapi.dev/api)",
    "ApiKey": "seu-token-de-acesso-aqui"
  }
}
```
---

### 3. Rodar a Aplicação

A aplicação deve ser executada via linha de comando com três parâmetros: o **ticker do ativo**, o **preço de venda** e o **preço de compra**.

1.  Abra o **Prompt de Comando** ou **PowerShell** na pasta onde o executável e o `appsettings.json` estão localizados.
2.  Execute o comando seguindo a sintaxe abaixo:

```bash
stock-quote-alert.exe <ATIVO> <PRECO_VENDA> <PRECO_COMPRA>
```