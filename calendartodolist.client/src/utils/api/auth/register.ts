export type ErrorInfo = { id: string; value: string };
type RegisterResponse =
  | {
      isSuccess: true;
    }
  | {
      isSuccess: false;
      title: string;
      errors: ErrorInfo[];
    };

export async function register(
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

  const errors: ErrorInfo[] = Object.keys(data.errors).map((k: string) => ({
    id: k,
    value: data.errors[k][0],
  }));

  return {
    isSuccess: false,
    title: data.title,
    errors,
  };
}
