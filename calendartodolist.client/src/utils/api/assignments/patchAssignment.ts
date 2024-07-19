import type { IResult } from "@/types/IResult";

export async function patchAssignment(
  id: string,
  isComplete: boolean
): Promise<IResult> {
  const body = JSON.stringify({ isComplete });
  const res = await fetch(`/api/Assignments/${id}`, {
    method: "PATCH",
    body,
    headers: {
      Authorization: "Bearer " + localStorage.getItem("accessToken"),
      "Content-Type": "application/json;charset=utf-8",
    },
  });

  return { isSuccess: res.ok };
}
