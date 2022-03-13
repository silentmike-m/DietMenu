<template>
  <tiptap-vuetify
    v-model="editorContent"
    :extensions="extensions"
    @input="$emit('input', arguments[0])"
  />
</template>

<script lang="ts">
import Vue from "vue";
import { onMounted, Ref, ref, watch } from "@nuxtjs/composition-api";
import {
  TiptapVuetify,
  Heading,
  Bold,
  Italic,
  Strike,
  Underline,
  BulletList,
  OrderedList,
  ListItem,
  Blockquote,
  HardBreak,
  History,
} from "tiptap-vuetify";

export default Vue.extend({
  name: "Editor",
  components: {
    TiptapVuetify,
  },
  data: () => ({
    extensions: [
      History,
      Bold,
      Italic,
      Underline,
      Strike,
      Blockquote,
      ListItem,
      BulletList,
      OrderedList,
      [Heading, { options: { levels: [1, 2, 3] } }],
      HardBreak,
    ],
  }),
  props: {
    content: String,
  },
  setup(props) {
    const editorContent: Ref<string> = ref("");

    watch(
      () => props.content,
      () => setEditorContent()
    );

    onMounted(() => {
      setEditorContent();
    });

    const setEditorContent = (): void => {
      editorContent.value = props.content as string;
    };

    return {
      editorContent,
    };
  },
});
</script>