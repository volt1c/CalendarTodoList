import type { IAssignment } from "@/types/IAssignment";
import type { IResult } from "@/types/IResult";

export async function putAssignment(assignment: IAssignment): Promise<IResult> {
  const body = JSON.stringify(assignment);
  const res = await fetch(`/api/Assignments/${assignment.id}`, {
    method: "PUT",
    body,
    headers: {
      Authorization: "Bearer " + localStorage.getItem("accessToken"),
      "Content-Type": "application/json;charset=utf-8",
    },
  });

  return { isSuccess: res.ok };
}
