# 📦 Code Pack CLI
> A powerful command-line tool for bundling code files into single packages

[![.NET](https://img.shields.io/badge/.NET-8.0+-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12+-239120?logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)

**Transform scattered code files into organized, single-file bundles with powerful customization options!**

## 🌟 Features

- 📁 **Multi-Language Support** - Bundle C#, Java, Python, JavaScript, C++, TypeScript, and 25+ more
- 🔤 **Smart Sorting** - Sort files by name or type
- 📝 **Source Annotations** - Add file path comments
- ✂️ **Empty Line Removal** - Clean up code formatting
- 👤 **Author Attribution** - Add author information
- 🎯 **Response Files** - Create reusable command configurations
- ⚡ **Fast Processing** - Efficiently handles large codebases

## 🚀 Quick Start

### Installation
```bash
# Download the latest release from GitHub
# Extract and add to PATH for global access
```

### Basic Usage
```bash
# Bundle all files with annotations and cleanup
fp bundle --l all --o bundle.txt --n --r --a "Author Name"

# Bundle specific language
fp bundle --l csharp --o project.cs

# Create interactive configuration
fp create-rsp
```

## 📖 Command Reference

### Bundle Command
```bash
fp bundle [OPTIONS]
```

**Required:**
- `--l <LANGUAGE>` - Language to include (`csharp`, `java`, `python`, `javascript`, `cpp`, `all`)

**Optional:**
- `--o <FILE>` - Output file path
- `--n` - Include source file paths as comments
- `--s <ORDER>` - Sort order: `name` or `type`
- `--r` - Remove empty lines
- `--a <NAME>` - Add author information

**Examples:**
```bash
# Complete bundle with all options
fp bundle --l all --o bundle.txt --n --r --a "Developer Name"

# C# project with sorting
fp bundle --l csharp --o project.cs --s type --n

# Clean JavaScript bundle
fp bundle --l javascript --o app.js --r
```

### Response File Command
```bash
fp create-rsp
```
Creates `bundle-command.rsp` for reusable configurations.

## 🎯 Supported Languages

| Language | Extensions | Alias |
|----------|------------|-------|
| **C#** | `.cs` | `csharp` |
| **Java** | `.java` | `java` |
| **Python** | `.py` | `python` |
| **JavaScript** | `.js` | `javascript` |
| **C++** | `.cpp`, `.h` | `cpp` |
| **All** | 25+ extensions | `all` |

## 💡 Use Cases

- 🎓 **Educational** - Submit assignments as single files
- 🤝 **Collaboration** - Share complete project context
- 📊 **Analysis** - Code review and documentation
- 🔬 **Development** - Package libraries and create snapshots

## 🛠️ Technical Details

- **Built with .NET 8.0** - Cross-platform support
- **Smart Filtering** - Excludes `bin/` and `debug/` directories
- **UTF-8 Support** - Handles international characters
- **Error Handling** - Comprehensive error messages

## 🔮 Roadmap

- [ ] Configuration files (JSON/YAML)
- [ ] Custom exclusion patterns
- [ ] Syntax highlighting output
- [ ] Watch mode for auto-rebuild
- [ ] Plugin system for custom processors

## 🤝 Contributing

1. Fork the repository
2. Create feature branch (`git checkout -b feature/name`)
3. Commit changes (`git commit -m 'Add feature'`)
4. Push and open Pull Request

## 🐛 Troubleshooting

**Usage Tips:**
- Sort parameter requires a value: `--s name` or `--s type`
- Output file path is always required: `--o yourfile.txt`
- Author names support various formats: `--a "Your Name"` or `--a Your-Name`

```bash
# Get help
fp --help
fp bundle --help
```

## 📄 License

MIT License - see [LICENSE](LICENSE) file for details.

## 👩‍💻 Author

**Batya Zilberberg** - [GitHub](https://github.com/Batya19)

---

<div align="center">

**Made with 💖 by a developer who believes in organized code**

*Bundle your code, streamline your workflow! 📦✨*

</div>