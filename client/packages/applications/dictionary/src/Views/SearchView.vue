<script setup lang="ts">
import SearchInputForm from "@/components/search/SearchInputForm.vue";
</script>

<template>
  <div class="view-container-search">
    <h2>{{ $t("searchDictionary") }}</h2>
    <search-input-form></search-input-form>
  </div>
</template>

<style scoped>
.view-container-search {
  width: 100%;
  height: 100%;
  margin-top: 10em;
  display: flex;
  flex-direction: column;
}

.view-container-search > * {
  margin: 0 auto;
}
</style>
