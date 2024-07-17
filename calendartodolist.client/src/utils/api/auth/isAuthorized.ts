export function isAuthorized(): boolean {
  const accessToken = localStorage.getItem("accessToken");

  if (accessToken == null) return false;

  const expiresIn = Number(localStorage.getItem("expiresIn"));
  const loginTime = Number(localStorage.getItem("loginTime"));

  const expirationTime = loginTime + expiresIn * 1000;

  if (expirationTime < Date.now()) return false;

  return true;
}
