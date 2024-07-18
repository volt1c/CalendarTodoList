type LoginResponse =
  | {
      isSuccess: false;
    }
  | {
      isSuccess: true;
      accessToken: string;
      expiresIn: number;
      loginTime: number;
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

  const loginTime = Date.now();

  localStorage.setItem("accessToken", data.accessToken);
  localStorage.setItem("expiresIn", data.expiresIn);
  localStorage.setItem("loginTime", loginTime.toString());

  return {
    isSuccess: true,
    accessToken: data.accessToken,
    expiresIn: data.expiresIn,
    loginTime,
  };
}
