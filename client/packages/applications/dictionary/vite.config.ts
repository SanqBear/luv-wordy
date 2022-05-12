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
  define: {
    __VUE_I18N_FULL_INSTALL__: true,
    __VUE_I18N_LEGACY_API__: false,
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
