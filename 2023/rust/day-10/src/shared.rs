use helpers::Point;
use std::collections::HashSet;

pub fn find_start(grid: &Vec<Vec<Pipe>>) -> Point {
    let mut start = Point::new(0, 0);
    'outer: for (y, row) in grid.iter().enumerate() {
        for (x, pipe) in row.iter().enumerate() {
            if *pipe == Pipe::Start {
                start = Point::new(x as i32, y as i32);
                break 'outer;
            }
        }
    }
    start
}

pub fn create_pipe_system(grid: &Vec<Vec<Pipe>>, start: Point) -> Vec<Point> {
    let mut visited = HashSet::new();
    let mut queue = Vec::new();
    queue.push(start);
    'outer: while let Some(point) = queue.pop() {
        let current_pipe = grid[point.y as usize][point.x as usize];
        for next_direction in current_pipe.get_next_directions() {
            let next = point + next_direction;
            if next.x < 0
                || next.y < 0
                || next.x >= grid[0].len() as i32
                || next.y >= grid.len() as i32
            {
                continue;
            }

            let next_pipe = grid[next.y as usize][next.x as usize];
            if next_pipe == Pipe::Start && queue.len() > 1 {
                visited.insert(point);
                queue.push(point);
                break 'outer;
            }
            // If we've already visited this point, ignore it
            if visited.contains(&next) {
                continue;
            }

            if next_pipe == Pipe::Ground {
                visited.insert(next);
                continue;
            }

            if next_direction == Point::UP && next_pipe.has_south_connection() {
                visited.insert(point);
                queue.push(point);
                queue.push(next);
                break;
            } else if next_direction == Point::DOWN && next_pipe.has_north_connection() {
                visited.insert(point);
                queue.push(point);
                queue.push(next);
                break;
            } else if next_direction == Point::LEFT && next_pipe.has_east_connection() {
                visited.insert(point);
                queue.push(point);
                queue.push(next);
                break;
            } else if next_direction == Point::RIGHT && next_pipe.has_west_connection() {
                visited.insert(point);
                queue.push(point);
                queue.push(next);
                break;
            }
        }
    }

    queue
}

#[derive(Debug, Clone, Copy, PartialEq, Eq, Hash)]
pub enum Pipe {
    Start,
    NorthAndSouth,
    EastAndWest,
    NorthAndEast,
    NorthAndWest,
    SouthAndWest,
    SouthAndEast,
    Ground,
}

impl From<char> for Pipe {
    fn from(value: char) -> Self {
        match value {
            'S' => Pipe::Start,
            '|' => Pipe::NorthAndSouth,
            '-' => Pipe::EastAndWest,
            'L' => Pipe::NorthAndEast,
            'J' => Pipe::NorthAndWest,
            '7' => Pipe::SouthAndWest,
            'F' => Pipe::SouthAndEast,
            '.' => Pipe::Ground,
            _ => panic!("Unknown pipe: {}", value),
        }
    }
}

impl Pipe {
    pub fn has_south_connection(&self) -> bool {
        match self {
            Pipe::NorthAndSouth => true,
            Pipe::SouthAndEast => true,
            Pipe::SouthAndWest => true,
            _ => false,
        }
    }

    pub fn has_north_connection(&self) -> bool {
        match self {
            Pipe::NorthAndSouth => true,
            Pipe::NorthAndEast => true,
            Pipe::NorthAndWest => true,
            _ => false,
        }
    }

    pub fn has_east_connection(&self) -> bool {
        match self {
            Pipe::EastAndWest => true,
            Pipe::NorthAndEast => true,
            Pipe::SouthAndEast => true,
            _ => false,
        }
    }

    pub fn has_west_connection(&self) -> bool {
        match self {
            Pipe::EastAndWest => true,
            Pipe::NorthAndWest => true,
            Pipe::SouthAndWest => true,
            _ => false,
        }
    }
}

impl Pipe {
    pub fn get_next_directions(&self) -> Vec<Point> {
        match self {
            Pipe::Start => vec![Point::UP, Point::DOWN, Point::LEFT, Point::RIGHT],
            Pipe::NorthAndSouth => vec![Point::UP, Point::DOWN],
            Pipe::EastAndWest => vec![Point::LEFT, Point::RIGHT],
            Pipe::NorthAndEast => vec![Point::UP, Point::RIGHT],
            Pipe::NorthAndWest => vec![Point::UP, Point::LEFT],
            Pipe::SouthAndWest => vec![Point::DOWN, Point::LEFT],
            Pipe::SouthAndEast => vec![Point::DOWN, Point::RIGHT],
            _ => panic!("Shouldn't be looking for next point"),
        }
    }
}

pub fn parse_input(input: &str) -> Vec<Vec<Pipe>> {
    input
        .lines()
        .map(|line| line.chars().map(|pipe| pipe.into()).collect::<Vec<Pipe>>())
        .collect()
}
