# Sistema de Destaque de Menu Ativo

## Visão Geral
Este sistema permite destacar visualmente o item de menu correspondente à página atual, facilitando a navegação dos usuários.

## Como Funciona

### 1. Estilos CSS
Os estilos CSS foram adicionados no arquivo `_Layout.cshtml` para destacar os itens de menu ativos:

```css
/* Estilos para itens de menu ativos */
.menu-item.active > .menu-link {
    background-color: rgba(105, 108, 255, 0.16) !important;
    color: #696cff !important;
    font-weight: 600;
}

.menu-item.active > .menu-link i {
    color: #696cff !important;
}
```

### 2. Menus Parciais
Os menus parciais (`_MenuFabricantePartial.cshtml`, `_MenuMecanicoPartial.cshtml`, `_MenuOficinaPartial.cshtml`) usam a classe `active` baseada no `ViewData["ActiveMenu"]`:

```razor
<li class="menu-item @(ViewData["ActiveMenu"] as string == "Dashboard" ? "active" : "")">
    <a asp-controller="Home" asp-action="Index" class="menu-link">
        <i class="menu-icon tf-icons bx bx-home-circle"></i>
        <div>Dashboard</div>
    </a>
</li>
```

### 3. Controllers
Cada action nos controllers define qual menu deve ser destacado através do `ViewData["ActiveMenu"]`:

```csharp
public async Task<IActionResult> Index()
{
    ViewData["ActiveMenu"] = "Dashboard";
    // resto do código...
}
```

## Mapeamento de Menus

### Menu Fabricante
| Nome do Menu | Valor ViewData["ActiveMenu"] | Controller | Action |
|--------------|----------------------------|------------|--------|
| Dashboard | `"Dashboard"` | Home | Index |
| Clientes | `"Clientes"` | Clientes | Index |
| Encomendas | `"Encomendas"` | Encomendas | Index |
| Ordens de Produção | `"OrdemProducao"` | OrdemProducao | Index |
| Gestão de Motas | `"GestaoMotas"` | Motas | Index |
| Gestão de Modelos | `"GestaoModelos"` | ModeloMotas | Index |
| Tipo de Checklists | `"TipoChecklists"` | Checklists | Index |
| Tipo de Documentos | `"TipoDocumentos"` | Documentos | Index |
| Gestão de Peças | `"GestaoPecas"` | Pecas | Index |
| Gestão de Fornecedores | `"GestaoFornecedores"` | Fornecedors | Index |
| Gestão de Material Recebido | `"GestaoMaterial"` | MaterialRecebidoes | Index |
| Gestão de Compras | `"GestaoCompras"` | Compras | Index |

### Menu Mecânico / Oficina
| Nome do Menu | Valor ViewData["ActiveMenu"] | Controller | Action |
|--------------|----------------------------|------------|--------|
| Histórico e Registo de Manutenções | `"HistoricoManutencoes"` | Servicos | Index |
| Calendário de Intervenções | `"CalendarioIntervencoes"` | Servicos | CalendarioIntervencoes |
| Meus Agendamentos | `"MeusAgendamentos"` | Mecanicos | MeusAgendamentos |
| Gestão de Mecânicos | `"GestaoMecanicos"` | Mecanicos | Index |

### Menus Globais
| Nome do Menu | Valor ViewData["ActiveMenu"] | Controller | Action |
|--------------|----------------------------|------------|--------|
| Enviar Mensagem | `"EnviarMensagem"` | Message | SendMessage |
| Testar Conexão | `"TestarConexao"` | Message | TestarConexao |

## Como Adicionar Novos Itens de Menu

### Passo 1: Atualizar o Menu Parcial
No arquivo do menu parcial apropriado (ex: `_MenuFabricantePartial.cshtml`), adicione o item com a classe `active`:

```razor
<li class="menu-item @(ViewData["ActiveMenu"] as string == "MeuNovoMenu" ? "active" : "")">
    <a asp-controller="MeuController" asp-action="MinhaAction" class="menu-link">
        <i class="menu-icon tf-icons bx bx-icon"></i>
        <div>Meu Novo Menu</div>
    </a>
</li>
```

### Passo 2: Definir no Controller
No controller correspondente, defina o `ViewData["ActiveMenu"]`:

```csharp
public async Task<IActionResult> MinhaAction()
{
    ViewData["ActiveMenu"] = "MeuNovoMenu";
    
    // resto do código...
    return View();
}
```

### Passo 3: Para Submenus
Se o item estiver dentro de um submenu (BackOffice), aplique o mesmo padrão:

```razor
<ul class="menu-sub">
    <li class="menu-item @(ViewData["ActiveMenu"] as string == "MeuSubmenu" ? "active" : "")">
        <a asp-controller="MeuController" asp-action="Index" class="menu-link">
            <div>Meu Submenu</div>
        </a>
    </li>
</ul>
```

## Notas Importantes

1. **Consistência**: Use sempre o mesmo valor de string tanto no menu quanto no controller
2. **Múltiplas Actions**: Se um controller tem múltiplas actions que devem destacar o mesmo menu, use o mesmo valor em todas elas
3. **Submenus**: Os submenus também recebem destaque adicional através da classe CSS específica `.menu-sub .menu-item.active`
4. **Case-sensitive**: Os valores são case-sensitive, então `"Dashboard"` é diferente de `"dashboard"`

## Personalização de Estilos

Para personalizar as cores e estilos do menu ativo, edite os estilos no arquivo `_Layout.cshtml`:

```css
/* Altere a cor de fundo */
.menu-item.active > .menu-link {
    background-color: rgba(105, 108, 255, 0.16) !important; /* Altere aqui */
}

/* Altere a cor do texto */
.menu-item.active > .menu-link {
    color: #696cff !important; /* Altere aqui */
}
```
