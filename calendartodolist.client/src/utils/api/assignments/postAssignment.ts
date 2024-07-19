import type { IAssignment } from "@/types/IAssignment";
import type { IResult } from "@/types/IResult";

type PostAssignmentResponse = IResult<{ assignment: IAssignment }>;

export async function postAssignment(
  title: string,
  date: string,
  description = ""
): Promise<PostAssignmentResponse> {
  const body = JSON.stringify({ title, date, description });
  const res = await fetch(`/api/Assignments`, {
    method: "POST",
    body,
    headers: {
      Authorization: "Bearer " + localStorage.getItem("accessToken"),
      "Content-Type": "application/json;charset=utf-8",
    },
  });

  if (!res.ok) return { isSuccess: false };

  const data = await res.json();

  return { isSuccess: true, assignment: data };
}
