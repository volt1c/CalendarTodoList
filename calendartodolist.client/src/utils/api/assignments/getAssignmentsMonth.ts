import type { IAssignment } from "@/types/IAssignment";

type GetAssignmentsResponse =
  | {
      isSuccess: false;
    }
  | {
      isSuccess: true;
      assignments: IAssignment[];
    };

export async function getAssignments(
  year: number,
  month: number
): Promise<GetAssignmentsResponse> {
  const res = await fetch(`/api/Assignments/${year}/${month}`, {
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
