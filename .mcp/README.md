# MCP (Model Context Protocol) Configuration

This directory contains MCP server configurations to help Claude remember context and improve over time.

## Configured Servers

### 1. Memory Server
- **Purpose**: Knowledge graph-based persistent memory system
- **Benefits**: Helps remember facts, relationships, and context across sessions
- **Usage**: Automatically stores and retrieves relevant information

### 2. Filesystem Server
- **Purpose**: Persist information in structured files
- **Location**: `.mcp/memory/` directory
- **Benefits**: Creates persistent knowledge base that survives between sessions

### 3. Sequential Thinking Server
- **Purpose**: Dynamic and reflective problem-solving
- **Benefits**: Helps develop structured thought processes and improve solutions

### 4. Azure MCP Server
- **Purpose**: Access to Azure services and Microsoft documentation
- **Benefits**: 
  - Direct access to Azure resources (Storage, Cosmos DB, Key Vault, etc.)
  - Query Azure Monitor logs and metrics
  - Access to Microsoft Learn documentation
  - Integration with Azure DevOps
- **Note**: Requires Azure credentials for full functionality

## How It Works

When Claude Code uses these MCP servers:
1. **Memory Server** maintains a knowledge graph of important information
2. **Filesystem Server** stores project context and patterns in markdown files
3. **Sequential Thinking** helps structure complex problem-solving

## Files Structure

```
.mcp/
├── claude_mcp.json     # MCP server configuration
├── memory/             # Persistent memory storage
│   ├── project_context.md   # Project-specific context
│   ├── code_patterns.md     # Common patterns and solutions
│   └── ...                  # Additional memory files
└── README.md           # This file
```

## Benefits for You

1. **Consistent Responses**: I'll remember your preferences and project details
2. **Improved Solutions**: I'll learn from past interactions and apply best practices
3. **Faster Help**: No need to re-explain context or preferences
4. **Better Code**: I'll follow established patterns in your codebase

## Maintenance

The memory files are version-controlled (except temporary/personal data) so:
- Project context is shared across team members
- Best practices are preserved
- Personal/sensitive data is excluded via .gitignore

## Usage

These servers are automatically activated when you use Claude Code. No manual intervention needed!