import { Link } from "./link";

export interface ApiResponse<T> {
  value: T;
  links: Link[];
}
