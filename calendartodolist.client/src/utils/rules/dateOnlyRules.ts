export const dateOnlyRules = [
  (value: any) => (value || "").length <= 20 || "Max 20 characters",
  (value: any) => {
    const pattern = /^[0-9]{4}-[0-9]{2}-[0-9]{2}$/;
    return pattern.test(value) || "Invalid date (YYYY-mm-dd).";
  },
];
