<script lang="ts">
import { useI18n } from "vue-i18n";
import SearchView from "./views/SearchView.vue";

export default {
  setup() {
    const { t } = useI18n();
    return { t };
  },
  components: {
    SearchView,
  },
};
</script>

<template>
  <search-view></search-view>
</template>
