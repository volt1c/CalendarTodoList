export function dateToJsonDateOnly(date: Date = new Date()) {
  return date.toJSON().slice(0, 10);
}
