use std::collections::HashSet;

#[derive(Debug, Clone, Copy, PartialEq, Eq, Hash)]
pub struct Point {
    pub x: i32,
    pub y: i32,
}

impl Point {
    pub fn new(x: i32, y: i32) -> Self {
        Self { x, y }
    }

    pub fn _add(&self, other: &Self) -> Self {
        Self::new(self.x + other.x, self.y + other.y)
    }

    pub fn _sub(&self, other: &Self) -> Self {
        Self::new(self.x - other.x, self.y - other.y)
    }

    pub fn _is_adjacent(&self, other: &Self) -> bool {
        let diff = self._sub(other);
        diff.x.abs() <= 1 && diff.y.abs() <= 1
    }

    pub fn get_adjacent(&self) -> Vec<Self> {
        let mut result = Vec::new();
        for x in self.x - 1..=self.x + 1 {
            for y in self.y - 1..=self.y + 1 {
                if x == self.x && y == self.y {
                    continue;
                }
                result.push(Self::new(x, y));
            }
        }
        result
    }
}

#[derive(Debug, Clone, Copy, PartialEq, Eq, Hash)]
pub struct Part {
    pub number: u32,
    pub point: Point,
    pub len: usize,
}

impl Part {
    pub fn all_adjacent(&self) -> HashSet<Point> {
        let mut points = HashSet::new();
        for x in self.point.x..=self.point.x + self.len as i32 - 1 {
            for adjacent in Point::new(x, self.point.y).get_adjacent() {
                points.insert(adjacent);
            }
        }

        points
    }
}

pub fn parse_parts(content: &Vec<Vec<char>>) -> Vec<Part> {
    let mut parts = Vec::new();
    for (y, line) in content.iter().enumerate() {
        let mut number = vec![];
        let mut start_x: i32 = 0;
        let mut start_y: i32 = 0;
        for (x, c) in line.iter().enumerate() {
            if c.is_ascii_digit() {
                number.push(*c);
                if number.len() == 1 {
                    start_x = x.try_into().unwrap();
                    start_y = y.try_into().unwrap();
                }
            } else if !number.is_empty() {
                let len = number.len();
                parts.push(Part {
                    number: number.iter().collect::<String>().parse::<u32>().unwrap(),
                    point: Point::new(start_x, start_y),
                    len,
                });
                number.clear();
            }
        }

        if !number.is_empty() {
            let len = number.len();
            parts.push(Part {
                number: number.iter().collect::<String>().parse::<u32>().unwrap(),
                point: Point::new(start_x, start_y),
                len,
            });
            number.clear();
        }
    }

    parts
}

// Write tests for Part
#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_part_all_adjacent() {
        let part = Part {
            number: 123,
            point: Point::new(0, 0),
            len: 1,
        };
        let expected = vec![
            Point::new(-1, -1),
            Point::new(-1, 0),
            Point::new(-1, 1),
            Point::new(0, -1),
            Point::new(0, 1),
            Point::new(1, -1),
            Point::new(1, 0),
            Point::new(1, 1),
        ];
        let actual = part.all_adjacent();
        assert_eq!(expected.len(), actual.len());
        for point in expected {
            assert!(actual.contains(&point));
        }
    }

    #[test]
    fn test_parse_parts() {
        let input = vec![vec!['1', '2', '3', '4', '5'], vec!['6', '7', '8', '9', '0']];
        let expected = vec![
            Part {
                number: 12345,
                point: Point::new(0, 0),
                len: 5,
            },
            Part {
                number: 67890,
                point: Point::new(0, 1),
                len: 5,
            },
        ];
        let actual = parse_parts(&input);
        assert_eq!(expected, actual);
    }
}
