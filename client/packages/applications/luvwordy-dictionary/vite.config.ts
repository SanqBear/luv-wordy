import { fileURLToPath } from "url";

import { defineConfig } from "vite";
import vue from "@vitejs/plugin-vue";

export default defineConfig({
  plugins: [vue()],
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", import.meta.url)),
    },
  },
  server: {
    port: 3001,
  },
  build: {
    manifest: true,
    rollupOptions: {
      input: "./main.ts",
    },
    outDir: "../../../dist/dictionary",
  },
});
