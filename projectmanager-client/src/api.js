import axios from "axios";

const baseURL = import.meta.env.VITE_API_BASE_URL || "https://localhost:5173/api";
const apiKey = import.meta.env.VITE_API_KEY;

export const api = axios.create({ baseURL });

api.interceptors.request.use(config => {
  if (apiKey) config.headers["X-API-KEY"] = apiKey;
  return config;
});

export const Projects = {
  list: (params) => api.get("/projects", { params }).then(r => r.data),
  get: (id) => api.get(`/projects/${id}`).then(r => r.data),
  create: (data) => api.post("/projects", data).then(r => r.data),
  update: (id, data) => api.put(`/projects/${id}`, data).then(r => r.data),
  remove: (id) => api.delete(`/projects/${id}`).then(r => r.status === 204),
};

export const Uploads = {
  image: async (file) => {
    const fd = new FormData();
    fd.append("file", file);
    const { data } = await api.post("/uploads/image", fd);
    return data.url; 
  }
};
