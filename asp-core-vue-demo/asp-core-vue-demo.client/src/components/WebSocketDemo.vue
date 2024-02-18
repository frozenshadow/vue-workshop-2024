<script setup>
import {onBeforeUnmount, onMounted, ref, nextTick} from 'vue';

const socket = ref(null);
const messages = ref([]);
const message = ref('');
const msgList = ref(null)

function sendMessage() {
  socket.value.send(message.value);
  message.value = '';
}

function startWS() {
  socket.value = new WebSocket('wss://localhost:7271/ws');

  socket.value.onmessage = async (event) => {
    const message = event.data;
    messages.value.push(message);

    await nextTick(); // Wait until DOM is updated by Vue
    msgList.value.scrollTop = msgList.value.scrollHeight;
  };

  socket.value.onopen = () => {
    console.log('WebSocket connection established');
  };

  socket.value.onerror = (error) => {
    console.error('WebSocket encountered an error:', error);
  };
}

function closeWS() {
  if (socket.value) {
    socket.value.close();
    socket.value = null;
  }
}

onMounted(() => {
  startWS();
});

onBeforeUnmount(() => {
  closeWS();
});
</script>

<template>
  <div>
    <ul ref="msgList" id="message-list">
      <li v-for="msg in messages">{{ msg }}</li>
    </ul>

    <div v-if="socket">
      <input v-model="message" @keyup.enter="sendMessage">
      <button @click="sendMessage">Send</button>
      <button @click="closeWS">Close WS</button>
    </div>
    <button v-else @click="startWS">Start WS</button>
  </div>
</template>

<style scoped>
#message-list {
  height: 200px;
  overflow: auto;
}
</style>
