#!/bin/bash
echo "Hook called with tool: $CLAUDE_TOOL_NAME" >> /home/daniel/Orderlyze.Sales/hook-test.log
echo "Tool input: $CLAUDE_TOOL_INPUT" >> /home/daniel/Orderlyze.Sales/hook-test.log
echo "---" >> /home/daniel/Orderlyze.Sales/hook-test.log