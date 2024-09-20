# TrueRealityPizza 项目开发与运行环境说明文档

## 目录

1. [开发环境介绍](#1-%E5%BC%80%E5%8F%91%E7%8E%AF%E5%A2%83%E4%BB%8B%E7%BB%8D)
   - 1.1 [必备工具](#11-%E5%BF%85%E5%A4%87%E5%B7%A5%E5%85%B7)
   - 1.2 [项目结构](#12-%E9%A1%B9%E7%9B%AE%E7%BB%93%E6%9E%84)
2. [运行环境搭建说明](#2-%E8%BF%90%E8%A1%8C%E7%8E%AF%E5%A2%83%E6%90%AD%E5%BB%BA%E8%AF%B4%E6%98%8E)
   - 2.1 [通过安装包直接运行](#21-%E9%80%9A%E8%BF%87%E5%AE%89%E8%A3%85%E5%8C%85%E7%9B%B4%E6%8E%A5%E8%BF%90%E8%A1%8C)
   - 2.2 [Azure 部署地址](#22-azure-%E9%83%A8%E7%BD%B2%E5%9C%B0%E5%9D%80)
   - 2.3 [克隆项目并安装依赖](#23-%E5%85%8B%E9%9A%86%E9%A1%B9%E7%9B%AE%E5%B9%B6%E5%AE%89%E8%A3%85%E4%BE%9D%E8%B5%96)
   - 2.4 [数据库配置](#24-%E6%95%B0%E6%8D%AE%E5%BA%93%E9%85%8D%E7%BD%AE)
   - 2.5 [配置启动应用](#25-%E9%85%8D%E7%BD%AE%E5%90%AF%E5%8A%A8%E5%BA%94%E7%94%A8)
   - 2.6 [运行客户端项目](#26-%E8%BF%90%E8%A1%8C%E5%AE%A2%E6%88%B7%E7%AB%AF%E9%A1%B9%E7%9B%AE)
   - 2.7 [用户认证与权限管理](#27-%E7%94%A8%E6%88%B7%E8%AE%A4%E8%AF%81%E4%B8%8E%E6%9D%83%E9%99%90%E7%AE%A1%E7%90%86)
   - 2.8 [前端组件开发](#28-%E5%89%8D%E7%AB%AF%E7%BB%84%E4%BB%B6%E5%BC%80%E5%8F%91)
3. [运行说明](#3-%E8%BF%90%E8%A1%8C%E8%AF%B4%E6%98%8E)
4. [注意事项](#4-%E6%B3%A8%E6%84%8F%E4%BA%8B%E9%A1%B9)
5. [常见问题](#5-%E5%B8%B8%E8%A7%81%E9%97%AE%E9%A2%98)

---

## 1. 开发环境介绍

### 1.1 必备工具

- **操作系统**: Windows 10 或更高版本，或支持 .NET 的其他操作系统（如 Linux 或 macOS）。
- **开发工具**:
  - **IDE**
    - Visual Studio 下载地址：[https://visualstudio.microsoft.com/](https://visualstudio.microsoft.com/)
    - Rider 下载地址：[Rider: The Cross-Platform .NET IDE from JetBrains](https://www.jetbrains.com/rider/)
  - **.NET Core SDK 8.0** 或更高版本(https://dotnet.microsoft.com/download)
  - **SQLite**: 本地数据库，用于数据存储。
- **浏览器**: 支持现代浏览器，如 Chrome、Edge 或 Firefox，确保能够正确调试 WebAssembly 应用。

### 1.2 项目结构

项目包含以下子项目，每个项目有不同的职责：

- **TrueRealityPizza**:
  
  - 负责应用程序的服务端逻辑，包括数据库上下文、控制器和 API。
  - 提供 Razor Components 支持，处理服务器端交互及身份认证逻辑。

- **TrueRealityPizza.Client**:
  
  - 使用 Blazor WebAssembly 构建的前端应用，负责用户界面展示和与服务端 API 的交互。

- **TrueRealityPizza.ComponentsLibrary**:
  
  - 提供可复用的 Blazor 组件库，封装前端页面中的交互逻辑。

- **TrueRealityPizza.Shared**:
  
  - 包含共享的模型和接口，例如 Pizza、订单等实体模型，以及仓储接口。

```plaintext
TrueRealityPizza/
├── TrueRealityPizza/                   # 服务端项目
├── TrueRealityPizza.Client/            # 客户端 WebAssembly 项目
├── TrueRealityPizza.ComponentsLibrary/ # 组件库
├── TrueRealityPizza.Shared/            # 共享库
```

## 2. 运行环境搭建说明

### 2.1 通过安装包直接运行

为了方便用户快速运行项目，已经将程序及其运行时环境打包在 **TrueRealityPizzaInstaller.exe** 文件中。可以直接通过该安装包进行项目安装并运行：

1. **下载并运行安装包**：
   双击 **TrueRealityPizzaInstaller.exe** 文件并双击运行。

2. **安装过程**：
   按照安装向导的提示，选择安装路径并进行安装。

3. **启动应用**：
   安装完成后，安装程序会自动启动 TrueRealityPizza 应用，你可以通过桌面快捷方式或者程序目录中的可执行文件启动应用。

安装完成后，应用将自动运行并打开浏览器指向 `https://localhost:5001`。你可以立即开始使用项目。

### 2.2 Azure 部署地址

项目已经部署在 Azure 云平台上，可以直接访问以下地址使用 TrueRealityPizza：

**网址**: [https://truerealitypizza.azurewebsites.net/](https://truerealitypizza.azurewebsites.net/)

无需在本地运行项目，只需通过浏览器访问上述网址或双击**TrueRealityPizzaWeb**，即可体验 TrueRealityPizza 应用的完整功能。

### 2.3 克隆项目并安装依赖

如果你希望手动搭建开发环境，可以按照以下步骤操作：

**克隆项目仓库**:

```bash
git clone https://github.com/RealTrueReality/TrueRealityPizza
cd TrueRealityPizza 
```

**克隆完仓库后，使用 `dotnet restore` 命令安装所有依赖包**：

```bash
dotnet restore
```

### 2.4 数据库配置

项目使用 **SQLite** 数据库。要初始化数据库并应用迁移，请运行以下命令：

```bash
dotnet ef database update
```

这将会创建数据库架构并插入初始数据（如用户角色、Pizza 数据等）。

### 2.5 配置启动应用

要启动服务器项目，运行以下命令：

```bash
dotnet run --project TrueRealityPizza
```

项目启动后，默认会使用 `https://localhost:5001` 作为服务器端地址，并自动在浏览器中打开该地址。

### 2.6 运行客户端项目

客户端项目使用 Blazor WebAssembly 构建，运行命令如下：

```bash
dotnet run --project TrueRealityPizza.Client
```

客户端应用将自动连接到服务端，并通过 API 获取数据。

### 2.7 用户认证与权限管理

项目通过 **ASP.NET Identity** 进行用户身份验证和权限管理。在 `Program.cs` 文件中定义了身份验证和角色管理的详细配置，包括密码要求和账户确认等。常见的身份验证规则包括：

- 密码至少 6 个字符，要求包含数字和小写字母。
- 用户注册后，必须通过电子邮件确认账户。

项目还支持角色管理，例如用户可以被分配为管理员或普通用户，来访问不同的功能模块。

### 2.8 前端组件开发

`TrueRealityPizza.ComponentsLibrary` 中包含可复用的 Blazor 组件，开发者可以通过修改这些组件，定制应用的前端功能。例如，`TemplatedDialog.razor` 和 `TemplatedList.razor` 组件提供了通用的对话框和列表展示功能。

## 3. 运行说明

项目启动后，前端和后端将分别通过 Blazor WebAssembly 和 ASP.NET Core 进行交互。服务端负责处理 API 请求、数据存储和身份认证，而客户端通过 HttpClient 与服务端 API 进行通信。

数据库操作通过 Entity Framework Core 进行管理，并将所有数据存储在 SQLite 数据库中。所有的数据交互都将在后端进行，而前端提供动态的用户界面。

## 4. 注意事项

- **开发模式调试**: 在开发模式下，项目启用了 **WebAssembly Debugging**，允许在浏览器中调试 Blazor 组件和后端 API 请求。
- **生产环境安全性**: 在生产模式下，应用将启用 **HSTS**（HTTP 严格传输安全）以及全局异常处理程序，确保应用在恶意攻击和异常情况中的安全性。
- **浏览器兼容性**: 建议使用最新版本的 Chrome、Edge 或 Firefox 浏览器，确保最佳的兼容性和性能。

## 5. 常见问题

### 5.1 数据库没有初始化怎么办？

如果在项目启动时数据库没有成功创建，请检查以下几点：

1. 确保已经运行 `dotnet ef database update` 命令来创建和更新数据库。
2. 检查项目中的 `PizzaStoreContext` 类，确保连接字符串正确配置为 `Data Source=pizza.db`。

### 5.2 无法访问本地部署的项目

1. 检查本地是否正确运行了 `dotnet run` 命令，确保服务器正常启动。
2. 如果遇到 HTTPS 错误，请确保你已经为本地开发环境配置了 SSL 证书，或将启动命令中的 URL 修改为 `http://localhost:5000`。
