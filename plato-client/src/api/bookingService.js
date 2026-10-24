import axios from "axios";

const API_BASE = "http://localhost:5051/api";

export const bookingService = {
  create: (payload, token) =>
    axios
      .post(`${API_BASE}/bookings`, payload, {
        headers: { Authorization: `Bearer ${token}` },
      })
      .then((r) => r.data),
};
