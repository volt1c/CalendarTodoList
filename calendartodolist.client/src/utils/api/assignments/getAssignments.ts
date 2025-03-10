import type { IResult } from "@/types/IResult";

export async function getAssignmentsToday(): Promise<IResult> {
  const res = await fetch(`/api/Assignments/today`, {
    method: "GET",
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
