export const authorisedFetch = async (endpoint: string, options: RequestInit = {}) => {
  const token = localStorage.getItem("token");

  let headers: Record<string, string> = {
    "Content-Type": "application/json",
    ...(options.headers as Record<string, string>),
  };

  if (token) {
    headers["Authorization"] = `Bearer ${token}`;
  }

  return fetch(`${endpoint}`, {
    ...options,
    headers,
  });
};
