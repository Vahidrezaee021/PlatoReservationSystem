import axios from "axios";

const API_BASE = "http://localhost:5051/api";

export const studioService = {
  getAll: () => axios.get(`${API_BASE}/studios`).then((r) => r.data),
  getById: (id) => axios.get(`${API_BASE}/studios/${id}`).then((r) => r.data),
  create: (payload, token) =>
    axios
      .post(`${API_BASE}/studios`, payload, {
        headers: { Authorization: `Bearer ${token}` },
      })
      .then((r) => r.data),
};
