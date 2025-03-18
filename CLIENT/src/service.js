import axios from 'axios';
axios.defaults.baseURL = "http://localhost:5010"

axios.interceptors.response.use(
  response => {
    return response;
  },
  error => {
    console.error('Error response:', error.response);
    return Promise.reject(error); 
  }
);
export default {
  getTasks: async () => {
    const result = await axios.get(`/getAll`)
    console.log(result);
    return result.data;
  },

  addTask: async (name) => {
    console.log('addTask', name);
    const result = await axios.post(`/addItem?name=${encodeURIComponent(name)}`);
    console.log(result);
    return result.data;
  },

  setCompleted: async (id) => {
    console.log('setCompleted', { id });
    const result = await axios.put(`/updateItem/${id}`, {});
    console.log(result);
    return result.data;
  },

  deleteTask: async (id) => {
    const result = await axios.delete(`/deleteItem/${id}`, {})
    console.log(result)
    return result.data;
  }
};
