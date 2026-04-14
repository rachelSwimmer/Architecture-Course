import { McpServer } from "@modelcontextprotocol/sdk/server/mcp.js";

const server = new McpServer({
  name: "Weather Service",
  version: "1.0.0",
});



import { z } from "zod";

server.tool(
  "getWeather","Get the current weather for a given city",
  { city: z.string() },
  async ({ city }) => {
    return {
      content: [
        {
          type: "text",
          text: `The weather in ${city} is sunny!`,
        },
      ],
    };
  },
);

import { StdioServerTransport } from "@modelcontextprotocol/sdk/server/stdio.js";

const transport = new StdioServerTransport();
await server.connect(transport);
