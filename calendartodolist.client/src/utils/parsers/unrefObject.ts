export function unrefObject(obj: any) {
  return JSON.parse(JSON.stringify(obj));
}
