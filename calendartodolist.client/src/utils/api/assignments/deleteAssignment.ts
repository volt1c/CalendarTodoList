import type { IResult } from "@/types/IResult";

export async function deleteAssignment(id: string): Promise<IResult> {
  const res = await fetch(`/api/Assignments/${id}`, {
    method: "DELETE",
    headers: {
      Authorization: "Bearer " + localStorage.getItem("accessToken"),
      "Content-Type": "application/json;charset=utf-8",
    },
  });

  return { isSuccess: res.ok };
}
