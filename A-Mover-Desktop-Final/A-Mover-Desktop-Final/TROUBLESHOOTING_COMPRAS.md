# Guia de Troubleshooting - Compras Index

## ✅ Verificações Realizadas

1. ✅ **Arquivo modificado corretamente**: `Views/Compras/Index.cshtml`
2. ✅ **Compilação bem-sucedida**
3. ✅ **Controller carregando dados**: `Include(c => c.Fornecedor).Include(c => c.Peca)`

## 🔧 Passos para Resolver Problema de Visualização

### 1️⃣ Limpar Cache do Navegador

**Chrome/Edge:**
```
Ctrl + Shift + Delete
→ Selecionar "Imagens e arquivos em cache"
→ Limpar dados
```

**Ou use:**
```
Ctrl + F5  (Hard Refresh)
```

### 2️⃣ Parar e Reiniciar a Aplicação

1. Pare a aplicação (Stop Debug)
2. Limpe a solução: `Build` → `Clean Solution`
3. Recompile: `Build` → `Rebuild Solution`
4. Execute novamente (F5)

### 3️⃣ Verificar se Está Acessando a URL Correta

Certifique-se de estar acessando:
```
https://localhost:XXXX/Compras/Index
ou
https://localhost:XXXX/Compras
```

### 4️⃣ Verificar Erros no Console do Navegador

1. Abra DevTools (F12)
2. Vá para a aba "Console"
3. Verifique se há erros JavaScript
4. Vá para a aba "Network"
5. Recarregue a página
6. Verifique se todos os recursos carregaram (CSS, JS, etc.)

### 5️⃣ Verificar se DataTables Está Carregado

No console do navegador (F12), digite:
```javascript
$.fn.DataTable
```

Se retornar `undefined`, o DataTables não está carregado.

### 6️⃣ Verificar _Layout.cshtml

Confirme que o `_Layout.cshtml` tem as referências corretas:

```html
<!-- jQuery -->
<script src="~/lib/jquery/dist/jquery.min.js"></script>

<!-- Bootstrap -->
<script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>

<!-- DataTables -->
<script src="https://cdn.datatables.net/2.1.8/js/dataTables.js"></script>

<!-- Boxicons -->
<link href="https://unpkg.com/boxicons@2.1.4/css/boxicons.min.css" rel="stylesheet">
```

## 📋 Checklist de Verificação

Marque cada item conforme verificar:

- [ ] Arquivo `Views/Compras/Index.cshtml` está modificado
- [ ] Aplicação foi recompilada
- [ ] Cache do navegador foi limpo
- [ ] Aplicação foi reiniciada
- [ ] URL correta está sendo acessada
- [ ] Não há erros no console do navegador
- [ ] DataTables está carregado
- [ ] jQuery está carregado
- [ ] Bootstrap está carregado
- [ ] Boxicons está carregado

## 🎯 Teste Rápido

Copie este código e cole no console do navegador (F12):

```javascript
console.log("jQuery:", typeof $);
console.log("Bootstrap:", typeof bootstrap);
console.log("DataTable:", typeof $.fn.DataTable);
console.log("Boxicons:", $('link[href*="boxicons"]').length);
```

**Resultado esperado:**
```
jQuery: function
Bootstrap: object
DataTable: function
Boxicons: 1
```

## 🔍 Diagnóstico de Problema

### Problema: Ainda está no estilo antigo

**Causa provável:** Cache do navegador

**Solução:**
1. Ctrl + F5 (hard refresh)
2. Ou limpe cache completamente
3. Ou abra em janela anônima

### Problema: Erros no console

**Possíveis erros e soluções:**

#### `$ is not defined`
- jQuery não está carregado
- Verifique `_Layout.cshtml`
- Confirme ordem: jQuery antes de outros scripts

#### `DataTable is not a function`
- DataTables não está carregado
- Adicione CDN do DataTables no `_Layout.cshtml`

#### `bootstrap is not defined`
- Bootstrap JS não está carregado
- Verifique referência no `_Layout.cshtml`

### Problema: Ícones não aparecem

**Causa:** Boxicons não carregado

**Solução:** Adicione no `<head>` do `_Layout.cshtml`:
```html
<link href="https://unpkg.com/boxicons@2.1.4/css/boxicons.min.css" rel="stylesheet">
```

## 🚀 Forçar Atualização Completa

Execute estes comandos no terminal do Visual Studio:

```bash
# Limpar bin e obj
dotnet clean

# Recompilar
dotnet build

# Executar
dotnet run
```

## 🔄 Última Opção: Verificar Arquivo Físico

1. Navegue até: `A-Mover-Desktop-Final\Views\Compras\Index.cshtml`
2. Abra com Notepad ou outro editor
3. Confirme que o conteúdo começa com:

```razor
@model IEnumerable<A_Mover_Desktop_Final.Models.Compras>

@{
    ViewData["Title"] = "Gestão de Compras";
}

<div class="card">
    <div class="card-body">
```

Se não estiver assim, o arquivo não foi salvo corretamente.

## 📞 Informações para Suporte

Se nada funcionar, forneça:

1. **URL acessada**: `_________________`
2. **Conteúdo do console (F12)**: `_________________`
3. **Screenshot da página**: `_________________`
4. **Versão do navegador**: `_________________`
5. **Erro específico que aparece**: `_________________`

## ✅ Confirmação de Sucesso

Quando funcionar, você verá:

✅ Título "Gestão de Compras" (não "Index")
✅ Botão "Criar Nova" no canto direito
✅ Card com sombra ao redor da tabela
✅ Ícones Boxicons (📦, 🏪, 📅)
✅ Badge azul na quantidade
✅ Botões coloridos (azul, amarelo, vermelho)
✅ Campo de pesquisa do DataTables
✅ Paginação na parte inferior

---

**Última atualização:** As alterações foram aplicadas com sucesso no arquivo.
O problema é de **cache do navegador** ou **aplicação não reiniciada**.

**Solução rápida:** 
1. Pare a aplicação (Stop)
2. Ctrl + F5 no navegador
3. Inicie a aplicação novamente (F5)
4. Acesse /Compras
