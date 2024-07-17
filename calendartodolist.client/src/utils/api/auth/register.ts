type RegisterResponse =
  | {
      isSuccess: true;
    }
  | {
      isSuccess: false;
      title: string;
      errors: Record<string, [string] | undefined>;
    };

export async function login(
  email: string,
  password: string
): Promise<RegisterResponse> {
  const body = JSON.stringify({ email, password });

  const res = await fetch("/api/Identity/register", {
    method: "POST",
    body,
    headers: {
      "Content-Type": "application/json;charset=utf-8",
    },
  });

  if (res.ok) return { isSuccess: true };

  const data = await res.json();

  return {
    isSuccess: false,
    title: data.title,
    errors: data.errors,
  };
}
