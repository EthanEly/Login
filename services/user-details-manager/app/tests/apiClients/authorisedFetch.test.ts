import { authorisedFetch } from "../../src/apiClients/authorisedFetch";

describe("authorisedFetch", () => {
  const originalFetch = global.fetch;
  const mockFetch = jest.fn();

  beforeAll(() => {
    global.fetch = mockFetch;
  });

  afterAll(() => {
    global.fetch = originalFetch;
  });

  beforeEach(() => {
    mockFetch.mockReset();
    mockFetch.mockResolvedValue({
      ok: true,
      json: async () => ({}),
    });
    localStorage.clear();
  });

  it("should call the correct API endpoint", async () => {
    await authorisedFetch("https://api.yourdomain.com/test-endpoint");

    expect(mockFetch).toHaveBeenCalledWith(
      "https://api.yourdomain.com/test-endpoint",
      expect.any(Object),
    );
  });

  it("should include Authorization header when token exists", async () => {
    const fakeToken = "abc-123-xyz";
    localStorage.setItem("token", fakeToken);

    await authorisedFetch("/protected");

    expect(mockFetch).toHaveBeenCalledWith(
      expect.any(String),
      expect.objectContaining({
        headers: expect.objectContaining({
          Authorization: `Bearer ${fakeToken}`,
          "Content-Type": "application/json",
        }),
      }),
    );
  });

  it("should NOT include Authorization header when token is missing", async () => {
    await authorisedFetch("/public");

    const fetchCalls = mockFetch.mock.calls[0];
    const options = fetchCalls[1];

    expect(options.headers).not.toHaveProperty("Authorization");
    expect(options.headers).toHaveProperty("Content-Type", "application/json");
  });

  it("should preserve custom headers passed in options", async () => {
    localStorage.setItem("token", "token");

    await authorisedFetch("/custom", {
      headers: { "X-Custom-Header": "MyValue" },
    });

    expect(mockFetch).toHaveBeenCalledWith(
      expect.any(String),
      expect.objectContaining({
        headers: expect.objectContaining({
          Authorization: "Bearer token",
          "X-Custom-Header": "MyValue",
        }),
      }),
    );
  });
});
