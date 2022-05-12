<script setup lang="ts"></script>

<template>
  <div class="component-container-search">
    <input
      type="text"
      class="component-input-search"
      :placeholder="$t('enterInputSearchKeyword')"
    />
    <input type="button" class="submit-button-search" :value="$t('search')" />
  </div>
</template>

<style scoped>
.component-container-search {
  display: flex;
  flex-direction: row;
}

.component-container-search > * {
  margin: 1em;
  border: 1px solid #e2e2e2;
  border-radius: 0.8em;
}

.component-container-search .component-input-search {
  height: 3em;
  width: 30em;
  padding-left: 1em;
}

.component-container-search .submit-button-search {
  height: 3.3em;
  width: 8em;
  cursor: pointer;
  background: white;
}

.component-container-search .submit-button-search:hover {
  background: #bae1ff;
}
</style>
