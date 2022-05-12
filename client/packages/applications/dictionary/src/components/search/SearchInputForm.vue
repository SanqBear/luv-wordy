<script setup lang="ts"></script>

<template>
  <div class="component-container-search">
    <input type="text" class="component-input-search" />
    <button class="submit-button-search">{{ $t("search") }}</button>
  </div>
</template>

<style scoped>
.component-container-search {
  display: flex;
  flex-direction: row;
}

.component-container-search .component-input-search {
  height: 4em;
  width: 30em;
  border: 1px solid #e2e2e2;
  border-radius: 0.8em;
  margin: 1em;
}

.component-container-search .submit-button-search {
  margin: 1em;
  padding: 0.5em;
  width: 2em;
}
</style>
