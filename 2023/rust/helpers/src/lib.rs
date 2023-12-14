use std::ops::{Add, Sub};

#[derive(Debug, Clone, Copy, PartialEq, Eq, Hash)]
pub struct Point {
    pub x: i32,
    pub y: i32,
}

impl Point {
    pub const UP: Point = Point { x: 0, y: -1 };
    pub const DOWN: Point = Point { x: 0, y: 1 };
    pub const LEFT: Point = Point { x: -1, y: 0 };
    pub const RIGHT: Point = Point { x: 1, y: 0 };
    pub const UP_LEFT: Point = Point { x: -1, y: -1 };
    pub const UP_RIGHT: Point = Point { x: 1, y: -1 };
    pub const DOWN_LEFT: Point = Point { x: -1, y: 1 };
    pub const DOWN_RIGHT: Point = Point { x: 1, y: 1 };

    pub fn new(x: i32, y: i32) -> Self {
        Self { x, y }
    }
    pub fn is_adjacent(&self, other: &Self) -> bool {
        let diff = *self - *other;
        diff.x.abs() <= 1 && diff.y.abs() <= 1
    }

    pub fn get_adjacent(&self, include_diagonals: bool) -> Vec<Self> {
        let mut result = Vec::new();
        result.push(*self + Self::UP);
        result.push(*self + Self::DOWN);
        result.push(*self + Self::LEFT);
        result.push(*self + Self::RIGHT);
        if include_diagonals {
            result.push(*self + Self::UP_LEFT);
            result.push(*self + Self::UP_RIGHT);
            result.push(*self + Self::DOWN_LEFT);
            result.push(*self + Self::DOWN_RIGHT);
        }
        result
    }
}

impl Add for Point {
    type Output = Self;

    fn add(self, other: Self) -> Self {
        Self::new(self.x + other.x, self.y + other.y)
    }
}

impl Sub for Point {
    type Output = Self;

    fn sub(self, other: Self) -> Self {
        Self::new(self.x - other.x, self.y - other.y)
    }
}
