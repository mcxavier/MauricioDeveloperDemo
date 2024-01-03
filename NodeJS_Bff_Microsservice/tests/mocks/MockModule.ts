export interface MockModule<T> {

  failure(): T;

  success(): T;
}
