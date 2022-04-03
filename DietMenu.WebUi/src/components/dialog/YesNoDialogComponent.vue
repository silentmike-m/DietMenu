<template>
  <div class="modal-body">{{ options.question }}</div>
  <div class="modal-footer">
    <button
      type="button"
      class="btn btn-primary"
      @click="options.confirmAction()"
    >
      TAK
    </button>
    <button
      type="button"
      class="btn btn-outline-danger"
      @click="options.cancelAction()"
    >
      NIE
    </button>
  </div>
</template>

<script lang="ts">
export default {
  props: ["options"],
};
</script>