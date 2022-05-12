import { createApp } from "vue";
import { createI18n } from "vue-i18n";
import messages from "./locales";

import App from "./src/App.vue";

const i18n = createI18n({
  legacy: false,
  locale: "ko",
  fallbackLocale: "en",
  messages,
});

createApp(App).use(i18n).mount("#app");
