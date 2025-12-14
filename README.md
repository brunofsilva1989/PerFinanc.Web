# 💰 PerFinanc — Gerenciador Financeiro Pessoal

O **PerFinanc** é um sistema de **gerenciamento financeiro pessoal** desenvolvido com **ASP.NET Core MVC**, pensado para substituir planilhas manuais e centralizar, de forma simples e organizada, o controle da vida financeira.

O foco do projeto é **clareza, praticidade e aprendizado sólido em .NET**, aplicando conceitos reais de mercado sem cair em soluções mágicas ou cópia e cola.

---

## 🎯 Objetivo do Projeto

O PerFinanc nasceu com dois propósitos principais:

1. **Resolver um problema real**: organizar receitas, despesas fixas, gastos variáveis e rendas extras em um único lugar.
2. **Aprofundar o domínio em .NET**: praticando MVC, validações, autenticação, relacionamento de entidades, boas práticas e evolução contínua do sistema.

---

## 🧩 Funcionalidades Implementadas

### 🔐 Autenticação e Usuários

* Cadastro de usuário
* Login e logout
* Integração com **ASP.NET Identity**
* Associação de dados financeiros por usuário logado

### 💸 Contas Fixas

* Cadastro de contas recorrentes (ex: aluguel, internet, academia)
* Definição de:

  * Nome da conta
  * Dia de vencimento
  * Valor padrão
  * Indicador se já vem descontada
* Relacionamento direto com o usuário autenticado

### 🧾 Lançamentos

* Geração de lançamentos mensais
* Controle de vencimento
* Base para alertas futuros

### 💼 Rendas Extras (Freelance)

* Cadastro de ganhos eventuais
* Valor
* Data de recebimento
* Categoria

### 🛒 Gastos Gerais

* Registro de despesas não fixas
* Controle por descrição, valor e data

### 📊 Estrutura pronta para evolução

* Base preparada para:

  * Dashboards
  * Relatórios
  * Alertas por e-mail
  * Comparativos mensais

---

## 🏗️ Arquitetura e Tecnologias

* **ASP.NET Core MVC**
* **Entity Framework Core**
* **SQL Server**
* **ASP.NET Identity**
* **Razor Views**
* **Bootstrap** para layout
* **Data Annotations** para validações

Estrutura pensada para manter:

* Separação de responsabilidades
* Código legível
* Facilidade de manutenção

---

## 🧠 Conceitos Praticados

Este projeto foi (e está sendo) usado para praticar na prática:

* MVC na vida real
* Modelagem de entidades
* Relacionamentos (1:N)
* Validações no backend
* Autenticação e autorização
* Scaffolding consciente (entendendo o que foi gerado)
* Evolução incremental do sistema

---

## 🚀 Próximas Funcionalidades (Roadmap)

* 📧 Alertas por e-mail para contas a vencer
* 📅 Geração automática de lançamentos mensais
* 📈 Dashboard financeiro
* 📊 Relatórios mensais e anuais
* 🏷️ Categorias personalizadas
* 📱 Melhorias visuais e UX

---

## 🧪 Status do Projeto

🟡 **Em desenvolvimento ativo**

O projeto está evoluindo de forma incremental, sempre priorizando entendimento, qualidade de código e decisões conscientes.

---

## 🤝 Contribuição

Este é um projeto pessoal de estudo e evolução, mas ideias e sugestões são sempre bem-vindas.

---

## ✍️ Autor

**Bruno Silva**
Desenvolvedor .NET apaixonado por aprendizado contínuo, projetos práticos e soluções bem pensadas.

---

Se você já sofreu com planilhas financeiras gigantes… o PerFinanc é exatamente o antídoto 😄
