type LoginResponse = {
  isSuccess: boolean;
};

export async function login(
  email: string,
  password: string
): Promise<LoginResponse> {
  const body = JSON.stringify({ email, password });

  const res = await fetch("/api/Identity/login", {
    method: "POST",
    body,
    headers: {
      "Content-Type": "application/json;charset=utf-8",
    },
  });

  if (!res.ok) return { isSuccess: false };

  const data = await res.json();

  localStorage.setItem("accessToken", data.accessToken);
  localStorage.setItem("expiresIn", data.expiresIn);
  localStorage.setItem("loginTime", Date.now().toString());

  return { isSuccess: true };
}
