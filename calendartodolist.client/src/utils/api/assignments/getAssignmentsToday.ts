import type { IAssignment } from "@/types/IAssignment";

type GetAssignmentsPendingResponse =
  | {
      isSuccess: false;
    }
  | {
      isSuccess: true;
      assignments: IAssignment[];
    };

export async function getAssignmentsPending(): Promise<GetAssignmentsPendingResponse> {
  const res = await fetch(`/api/Assignments/pending`, {
    method: "GET",
    headers: {
      Authorization: "Bearer " + localStorage.getItem("accessToken"),
      "Content-Type": "application/json;charset=utf-8",
    },
  });

  if (!res.ok) return { isSuccess: false };

  const data = await res.json();

  return { isSuccess: true, assignments: data };
}
