export type IResult<S = {}, F = {}> =
  | ({
      isSuccess: true;
    } & S)
  | ({
      isSuccess: false;
    } & F);
